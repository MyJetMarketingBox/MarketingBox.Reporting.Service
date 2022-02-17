BEGIN TRANSACTION;

CREATE TEMPORARY TABLE report_by_country
(
    Id                bigint GENERATED BY DEFAULT AS IDENTITY,
    GroupedBy         text NOT NULL,
    Name              text,
    RegistrationCount int,
    FtdCount          int,
    FailedCount       int,
    UnassignedCount   int,
    Revenue           decimal,
    Payout            decimal,
    Epc               decimal,
    Clicks            decimal,
    Pl                decimal,
    Cr                decimal,
    Epl               decimal,
    Roi               decimal,
    CONSTRAINT PK_report_by_affiliate PRIMARY KEY (Id)
) on commit drop;

with aggregate_by_country as (
    select rd."Country"                                       as GroupedBy,
           rd."Country"                                       as Name,
           rd."BrandId"                                       as Brand,
           count(*) filter ( where rd."Status" = 3 )          as FtdCount,
           count(*) filter ( where rd."Status" in (1, 2, 3) ) as RegistrationCount,
           count(*) filter ( where rd."Status" = 0 )          as FailedCount,
           count(*) filter ( where rd."Status" = 4 )          as UnassignedCount,
           case br."PayoutPlan"
               when 0 then br."PayoutAmount" --CPA Plan for deposit payout
               else 0
               end                                            as DepPayout,
           case br."RevenuePlan"
               when 0 then br."RevenueAmount" --CPA Plan for deposit revenue
               else 0
               end                                            as DepRevenue,
           case br."PayoutPlan"
               when 1 then br."PayoutAmount" --CPL Plan for lead payout
               else 0
               end                                            as LeadPayout,
           case br."RevenuePlan"
               when 1 then br."RevenueAmount" --CPL Plan for lead revenue
               else 0
               end                                            as LeadRevenue
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
              when @TenantId is not null then
                  rd."TenantId" = @TenantId
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
             Brand,
             DepPayout,
             DepRevenue,
             LeadPayout,
             LeadRevenue)

INSERT
INTO report_by_country(GroupedBy,
                       Name,
                       FtdCount,
                       RegistrationCount,
                       FailedCount,
                       UnassignedCount,
                       Revenue,
                       Payout)
select rd.GroupedBy                                                             as GroupedBy,
       rd.Name                                                                  as Name,
       sum(rd.FtdCount)                                                         as FtdCount,
       sum(rd.RegistrationCount)                                                as RegistrationCount,
       sum(rd.FailedCount)                                                      as FailedCount,
       sum(rd.UnassignedCount)                                                  as UnassignedCount,
       sum(rd.FtdCount * rd.DepRevenue + rd.RegistrationCount * rd.LeadRevenue) as Revenue,
       sum(rd.FtdCount * rd.DepPayout + rd.RegistrationCount * rd.LeadPayout)   as Payout

from aggregate_by_country rd
group by GroupedBy, Name
order by GroupedBy;

update report_by_country
set Pl  = Revenue - Payout,
    Cr  = case when RegistrationCount != 0 then cast(FtdCount as float) / RegistrationCount * 100 end,
    Epl = case when RegistrationCount != 0 then Revenue / RegistrationCount end,
    Roi = case when Payout != 0 then Revenue / Payout * 100 end;

select *
from report_by_country
where case @asc
          when true
              then id > @cursor
          else id < @cursor end
order by case when @asc = true then id end,
         case when @asc = false then id end desc
limit @limit;
