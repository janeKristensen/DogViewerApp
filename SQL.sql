Create table Dogs (
	Id int Identity(1,1),
	BreedName varchar(100),
	SubBreed varchar(100),
	CoatLength varchar(20),
	Size varchar(20),
	Temper varchar(20),
	ExcersizeLevel tinyint,
	AverageAge float,
	primary key(Id)
);