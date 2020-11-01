--Create a database with name as 'karomi' and execute the below script

--Roles
CREATE TABLE Role(
RoleId int identity(1,1) primary key,
Role varchar(50) not null
);

insert into Role values('HR');
insert into Role values('PM');
insert into Role values('TL');
insert into Role values('DEV');

--Projects
CREATE TABLE Project (
ProjectId int identity(1,1) primary key,
ProjectName varchar(250) not null,
);

insert into Project values ('Proj1');
insert into Project values ('Proj2');

--Teams
CREATE TABLE Team (
TeamId int identity(1,1) primary key,
TeamName varchar(250) not null,
ProjectId int  references Project(ProjectId)
);

insert into Team values ('Team1.1',1);
insert into Team values ('Team2.1',1);
insert into Team values ('Team1.2',2);
insert into Team values ('Team2.2',2);

--Employees
CREATE TABLE Employee (
EmployeeId int identity(1,1) primary key,
Email varchar(250) not null,
Name varchar(250) not null,
MobileNumber varchar(10) not null,
RoleId int references Role(RoleId) not null
);

insert into Employee values('hradmin@karomi.com','ImHR',9999999999,1);
insert into Employee values('pm1@karomi.com','ImPM',9999999999,2);
insert into Employee values('tl1@karomi.com','Imtl',9999999999,3);

--Employee Teams
CREATE TABLE EmployeeProjTeamXref (
EmployeeProjXrefId int identity(1,1) primary key,
EmployeeId int references Employee(EmployeeId),
ProjectId int references Project(ProjectId),
TeamId int references Team(TeamId)
);

insert into EmployeeProjTeamXref values(1,1,null);
insert into EmployeeProjTeamXref values(2,1,null);
insert into EmployeeProjTeamXref values(3,1,1);

select * from Role
select * from Project
select * from Team
select * from Employee
select * from EmployeeProjTeamXref
