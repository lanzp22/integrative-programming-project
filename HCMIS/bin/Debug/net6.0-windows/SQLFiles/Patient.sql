CREATE TABLE PatientInformation(
	ID				SMALLINT		PRIMARY KEY		IDENTITY(1, 1),
	Fullname		VARCHAR(100)	NOT NULL,
	Gender			VARCHAR(6)		NOT NULL,
	Birthday		DATE			NOT NULL,
	Address			VARCHAR(255)	NOT NULL,
	Bloodtype		VARCHAR(3)		NOT NULL,
	MartialStatus	VARCHAR(9)		NOT NULL
);
CREATE TABLE PatientContact(
	ID			SMALLINT		FOREIGN KEY		REFERENCES PatientInformation(ID) ON DELETE CASCADE,
	Email		VARCHAR(320)	NOT NULL,
	PhoneNumber VARCHAR(12)		NOT NULL
);