CREATE OR REPLACE FUNCTION GetAllUsers()
RETURNS TABLE(id INT, login VARCHAR, password VARCHAR, name VARCHAR, surname VARCHAR, age INT) AS
$$
BEGIN
RETURN QUERY
SELECT Id, Login, Password, Name, Surname, Age
FROM Users;
END;
$$ LANGUAGE plpgsql;
