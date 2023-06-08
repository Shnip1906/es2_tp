create table perfil(
    id_perfil uuid constraint perfil_pk primary key default uuid_generate_v4(),
    nome_perfil varchar(100) not null,
    pais varchar(100) not null,
    email varchar(100) not null,
    precoHora float not null,
    publico boolean default false,
    id_experiencia uuid,
        FOREIGN KEY (id_experiencia) REFERENCES experiencia (id_experiencia)
);