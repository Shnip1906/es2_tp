create table perfil(
                       id_perfil uuid constraint perfil_pk primary key default uuid_generate_v4(),
                       nome_perfil varchar(100) not null,
                       pais varchar(100) not null,
                       email varchar(100) not null,
                       precoHora float not null,
                       publico boolean default false
);

create table experiencia(
    id_experiencia uuid primary key default uuid_generate_v4(),
    nome_experiencia varchar(100) not null,
    nome_empresa varchar(100) not null,
    anoInicio int not null,
    anoFim int not null,
    continuo boolean default false,
    id_perfil uuid,
    FOREIGN KEY (id_perfil) REFERENCES perfil (id_perfil)
);