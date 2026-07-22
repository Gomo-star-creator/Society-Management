create database SocietyDB;
use SocietyDB;

create table Users (
	Id int auto_increment primary key,
    Username varchar(50) unique not null,
    PasswordHash varchar(255) not null,
    Role varchar(20) not null default 'Member' -- 'Admin' or 'Member'
);

create table Members (
	Id int auto_increment primary key,
    FullName varchar(100) not null,
    Email varchar(100) unique not null,
    StudentNumber varchar(20) unique not null,
    JoinDate date not null,
    UserId int null,
    foreign key (UserId) references Users(Id)
);


create table Events (
	Id int auto_increment primary key,
    Title varchar (150) not null,
    Description text,
    EventDate datetime not null,
    Location varchar(200)
);

create table Attendance (
	Id int auto_increment primary key,
    EventId int not null,
    MemberId int not null,
    Attended boolean default false,
    foreign key (EventId) references Events(Id),
    foreign key (MemberId) references Members(Id),
    unique key unique_attendance (EventId, MemberId)
);

ALTER TABLE Attendance RENAME TO Attendances;

SHOW TABLES;

SELECT * FROM Users;

