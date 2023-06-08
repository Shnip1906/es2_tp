create table perfil(
    id_perfil uuid constraint experiencia_pk primary key default uuid_generate_v4(),
    nome_perfil varchar(100) not null,
    pais varchar(100) not null,
    email varchar(100) not null,
    precoHora float not null,
    publico boolean default false,
    experiecia_id uuid constraint experiencia_perfil_id_fk references experiencia on
        delete cascade
);