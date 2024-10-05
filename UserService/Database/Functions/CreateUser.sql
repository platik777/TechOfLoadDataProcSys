CREATE OR REPLACE FUNCTION CreateUser(
    p_login VARCHAR,
    p_password VARCHAR,
    p_name VARCHAR,
    p_surname VARCHAR,
    p_age INT
)
    RETURNS INT AS
$$
DECLARE
    new_id INT;
BEGIN
    INSERT INTO users (Login, Password, Name, Surname, Age)
    VALUES (p_login, p_password, p_name, p_surname, p_age)
    RETURNING Id INTO new_id;

    RETURN new_id;
END;
$$ LANGUAGE plpgsql;
