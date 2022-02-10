BEGIN TRANSACTION;

CREATE TEMPORARY TABLE report_by_day
(
    Id                bigint GENERATED BY DEFAULT AS IDENTITY,
    GroupedBy         timestamp NOT NULL,
    Name              text,
    RegistrationCount int,
    FtdCount          int,
    Revenue           int,
    Payout            int,
    CONSTRAINT PK_report_brand PRIMARY KEY (Id)
) on commit drop;
CREATE TEMPORARY TABLE aggregate_by_day
(
    Id                bigint GENERATED BY DEFAULT AS IDENTITY,
    GroupedBy         timestamp NOT NULL,
    Name              text,
    BrandId           int,
    RegistrationCount int,
    FtdCount          int,
    DepPayout         int,
    LeadPayout        int,
    DepRevenue        int,
    LeadRevenue       int,
    CONSTRAINT PK_aggregate_by_brand PRIMARY KEY (Id)
) on commit drop;

INSERT INTO aggregate_by_day
(GroupedBy,
 Name,
 BrandId,
 FtdCount,
 RegistrationCount,
 DepPayout,
 DepRevenue,
 LeadPayout,
 LeadRevenue)

select date(rd."CreatedAt") as GroupedBy,
       date(rd."CreatedAt") as Name,
       rd."BrandId",
       count(*) filter ( where rd."Status" = 3 ),
       count(*) filter ( where rd."Status" = 1 ),
       case br."PayoutPlan"
           when 0 then br."PayoutAmount" --CPA Plan for deposit payout
           else 0
           end as DepPayout,
       case br."RevenuePlan"
           when 0 then br."RevenueAmount" --CPA Plan for deposit revenue
           else 0
           end as DepRevenue,
       case br."PayoutPlan"
           when 1 then br."PayoutAmount" --CPL Plan for lead payout
           else 0
           end as LeadPayout,
       case br."RevenuePlan"
           when 1 then br."RevenueAmount" --CPL Plan for lead revenue
           else 0
           end as LeadRevenue
from "reporting-service".registrations_details rd
         join "reporting-service".brands br
              on br."Id" = rd."BrandId" and
                 br."TenantId" = rd."TenantId"
where case
          when @AffiliateId is not null then
                  rd."AffiliateId" = @AffiliateId
          else true
    end
  and case
          when @Country is not null then
                  rd."Country" = @Country
          else true
    end
  and case
          when @BrandId is not null then
                  rd."BrandId" = @BrandId
          else true
    end
  and case
          when @FromDate is not null then
                  rd."CreatedAt" >= @FromDate
          else true
    end
  and case
          when @ToDate is not null then
                  rd."CreatedAt" <= @ToDate
          else true
    end
group by GroupedBy,
         Name,
         rd."BrandId",
         DepPayout,
         DepRevenue,
         LeadPayout,
         LeadRevenue;

INSERT INTO report_by_day
(GroupedBy, Name, FtdCount, RegistrationCount, Revenue, Payout)
select rd.GroupedBy,
       rd.Name,
       sum(rd.FtdCount),
       sum(rd.RegistrationCount),
       sum(rd.FtdCount * rd.DepRevenue + rd.RegistrationCount * rd.LeadRevenue),
       sum(rd.FtdCount * rd.DepPayout + rd.RegistrationCount * rd.LeadPayout)
from aggregate_by_day rd
group by rd.GroupedBy, rd.Name
order by rd.GroupedBy;

select *
from report_by_day
where case @asc
          when true
              then id > @cursor
          else id < @cursor end
order by case when @asc = true then id end,
         case when @asc = false then id end desc
limit @limit;

COMMIT TRANSACTION;
