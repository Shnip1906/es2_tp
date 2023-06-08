create table experiencia(
    id uuid constraint experiencia_pk primary key default uuid_generate_v4(),
    nome_experiencia varchar(100) not null,
    anoInicio int not null,
    anoFim int not null,
    continuo boolean default false
);