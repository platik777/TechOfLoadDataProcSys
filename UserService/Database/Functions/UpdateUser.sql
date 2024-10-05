CREATE OR REPLACE FUNCTION UpdateUser(p_id INT, p_password VARCHAR, p_name VARCHAR, p_surname VARCHAR, p_age INT)
RETURNS VOID AS
$$
BEGIN
UPDATE users u
SET Password = p_password, Name = p_name, Surname = p_surname, Age = p_age
WHERE u.Id = p_id;
END;
$$ LANGUAGE plpgsql;