-- 1
USE TurnBasedRPG
GO

select username, PasswordHash
from dbo.Users
where PasswordHash in (
    select PasswordHash
    from dbo.Users
    group by PasswordHash
    having count(*) > 1
)
-- 3

select 
    case 
        when level between 1 and 40 then '1-40 beginner'
        when level between 41 and 70 then '41-70 advnced'
        when level between 71 and 80 then '71-80expert'
        when level between 81 and 99 then '81-99 maseter'
		else '100'
    end as range,
    count(CharacterId) as 'count'
from dbo.Characters
group by 
    casE 
        when level between 1 and 40 then '1-40 beginner'
        when level between 41 and 70 then '41-70 advnced'
        when level between 71 and 80 then '71-80expert'
        when level between 81 and 99 then '81-99 maseter'
		else '100'
    end

-- 2

select  u.CountryCode, count(u.Id) as arutamashiat
from dbo.Users as u
left join dbo.Characters as c on u.Id = c.UserId
where c.CharacterId IS NULL
group by u.CountryCode

-- 4

select chars.CharacterCustomName, u.username, chars.UnallocatedPoints from dbo.Users as u
join dbo.Characters as chars on u.Id = chars.UserId
where chars.UnallocatedPoints > 0
order by chars.UnallocatedPoints desc

-- 5

select u.mail, c.CharacterCustomName, u.username, c.level
from dbo.Users as u
join dbo.Characters as c on u.Id = c.UserId
where
    c.CharacterCustomName LIKE '%' + u.Username + '%'
    OR
    c.CharacterCustomName LIKE '%' 
        + left(u.mail, CHARINDEX('@', u.mail) - 1) 
        + '%'
order by c.level desc;

-- 6

select chars.CharacterCustomName, chars.Experience as curexp, chars.level as cirlevel, sm.ExperienceRequired as next, (sm.ExperienceRequired - chars.Experience) as expgap
from dbo.Characters as chars
inner join dbo.LevelRequirements as sm on sm.level =1+ chars.level
where (sm.ExperienceRequired - chars.Experience) <= 10
order by expgap desc

-- 7

declare @hpp int = (select hp from dbo.Foes where level >= 100)
select u.username, sum(C.Strength) as total, @hpp as bosshp
from dbo.Users as u
join dbo.Characters as c on u.Id = C.UserId
group by u.Username
having sum(c.Strength) > @hpp