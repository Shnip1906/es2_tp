create table experiencia(
    id_experiencia uuid primary key default uuid_generate_v4(),
    nome_experiencia varchar(100) not null,
    nome_empresa varchar(100) not null,
    anoInicio int not null,
    anoFim int not null,
    continuo boolean default false
);