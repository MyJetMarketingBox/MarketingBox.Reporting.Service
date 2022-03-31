BEGIN TRANSACTION;
CREATE TEMPORARY TABLE aggregation
(
    Id                bigint GENERATED BY DEFAULT AS IDENTITY,
    GroupedBy         bigint,
    Name              text,
    BrandId           bigint,
    RegistrationCount int,
    FtdCount          int,
    FailedCount       int,
    UnassignedCount   int,
    DepPayout         decimal,
    LeadPayout        decimal,
    DepRevenue        decimal,
    LeadRevenue       decimal,
    CONSTRAINT PK_aggregation PRIMARY KEY (Id)
) on commit drop;
CREATE TEMPORARY TABLE aggregation_of_registrations
(
    Id                bigint GENERATED BY DEFAULT AS IDENTITY,
    GroupedBy         bigint,
    Name              text,
    BrandId           bigint,
    RegistrationCount int,
    FailedCount       int,
    UnassignedCount   int,
    DepPayout         decimal,
    LeadPayout        decimal,
    DepRevenue        decimal,
    LeadRevenue       decimal,
    CONSTRAINT PK_aggregation_of_registrations PRIMARY KEY (Id)
) on commit drop;
CREATE TEMPORARY TABLE aggregation_of_ftds
(
    Id          bigint GENERATED BY DEFAULT AS IDENTITY,
    GroupedBy   bigint,
    Name        text,
    BrandId     bigint,
    FtdCount    int,
    DepPayout   decimal,
    LeadPayout  decimal,
    DepRevenue  decimal,
    LeadRevenue decimal,
    CONSTRAINT PK_aggregation_of_ftds PRIMARY KEY (Id)
) on commit drop;
CREATE TEMPORARY TABLE report
(
    Id                bigint GENERATED BY DEFAULT AS IDENTITY,
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
    CONSTRAINT PK_report PRIMARY KEY (Id)
) on commit drop;

/*Create table with aggregated data by CreatedAt to count amount of registrations*/
INSERT INTO aggregation_of_registrations
(GroupedBy,
 Name,
 BrandId,
 RegistrationCount,
 FailedCount,
 UnassignedCount,
 DepPayout,
 DepRevenue,
 LeadPayout,
 LeadRevenue)
select rd."AffiliateId"                                   as GroupedBy,
       rd."AffiliateName"                                 as Name,
       rd."BrandId"                                       as Brand,
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
         LeadRevenue;

/*Create table with aggregated data by ConversionDate to count amount of Ftds*/
INSERT INTO aggregation_of_ftds
(GroupedBy,
 Name,
 BrandId,
 FtdCount,
 DepPayout,
 DepRevenue,
 LeadPayout,
 LeadRevenue)
select rd."AffiliateId"                          as GroupedBy,
       rd."AffiliateName"                        as Name,
       rd."BrandId"                              as Brand,
       count(*) filter ( where rd."Status" = 3 ) as FtdCount,
       case br."PayoutPlan"
           when 0 then br."PayoutAmount" --CPA Plan for deposit payout
           else 0
           end                                   as DepPayout,
       case br."RevenuePlan"
           when 0 then br."RevenueAmount" --CPA Plan for deposit revenue
           else 0
           end                                   as DepRevenue,
       case br."PayoutPlan"
           when 1 then br."PayoutAmount" --CPL Plan for lead payout
           else 0
           end                                   as LeadPayout,
       case br."RevenuePlan"
           when 1 then br."RevenueAmount" --CPL Plan for lead revenue
           else 0
           end                                   as LeadRevenue
from "reporting-service".registrations_details rd
         join "reporting-service".brands br
              on br."Id" = rd."BrandId" and
                 br."TenantId" = rd."TenantId"
where rd."ConversionDate" is not null
  and case
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
              rd."ConversionDate" >= @FromDate
          else true
    end
  and case
          when @ToDate is not null then
              rd."ConversionDate" <= @ToDate
          else true
    end
group by GroupedBy,
         Name,
         Brand,
         DepPayout,
         DepRevenue,
         LeadPayout,
         LeadRevenue;

/*Create table to combine aggregated data for registrations and ftds*/
insert into aggregation(GroupedBy,
                        Name,
                        BrandId,
                        RegistrationCount,
                        FtdCount,
                        FailedCount,
                        UnassignedCount,
                        DepPayout,
                        LeadPayout,
                        DepRevenue,
                        LeadRevenue)
select coalesce(t1.GroupedBy, t2.GroupedBy)     as GroupedBy,
       coalesce(t1.Name, t2.Name)               as Name,
       coalesce(t1.BrandId, t2.BrandId)         as Brand,
       coalesce(t1.RegistrationCount, 0)        as RCount,
       coalesce(t2.FtdCount, 0)                 as FCount,
       coalesce(t1.FailedCount, 0)              as FailCount,
       coalesce(t1.UnassignedCount, 0)          as UnCount,
       coalesce(t1.DepPayout, t2.DepPayout)     as DPayout,
       coalesce(t1.LeadPayout, t2.LeadPayout)   as LPayout,
       coalesce(t1.DepRevenue, t2.DepRevenue)   as DRevenue,
       coalesce(t1.LeadRevenue, t2.LeadRevenue) as LRevenue

from aggregation_of_registrations t1
         full join aggregation_of_ftds t2 on
            t1.GroupedBy = t2.GroupedBy and
            t1.Name = t2.Name and
            t1.BrandId = t2.BrandId;

/*Create result table and calculate Payout and Revenue based on proper aggregated data*/
insert into report(Name,
                   RegistrationCount,
                   FtdCount,
                   FailedCount,
                   UnassignedCount,
                   Payout,
                   Revenue)
select rd.Name                                                                  as Name,
       sum(rd.RegistrationCount)                                                as RegistrationCount,
       sum(rd.FtdCount)                                                         as FtdCount,
       sum(rd.FailedCount)                                                      as FailedCount,
       sum(rd.UnassignedCount)                                                  as UnassignedCount,
       sum(rd.FtdCount * rd.DepPayout + rd.RegistrationCount * rd.LeadPayout)   as Payout,
       sum(rd.FtdCount * rd.DepRevenue + rd.RegistrationCount * rd.LeadRevenue) as Revenue
from aggregation rd
group by GroupedBy, Name
order by GroupedBy;

/*Calculate business values*/
update report
set Pl  = Revenue - Payout,
    Cr  = case when RegistrationCount != 0 then cast(FtdCount as float) / RegistrationCount * 100 end,
    Epl = case when RegistrationCount != 0 then Revenue / RegistrationCount end,
    Roi = case when Payout != 0 then Revenue / Payout * 100 end;

/*paginate result*/
select *
from report
where case @asc
          when true
              then id > @cursor
          else id < @cursor end
order by case when @asc = true then id end,
         case when @asc = false then id end desc
limit @limit;

COMMIT TRANSACTION;
