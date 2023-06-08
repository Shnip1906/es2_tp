create table skillProf(
    id_skillsProf uuid constraint skillProf_pk primary key default uuid_generate_v4(),
    nHoras int not null,
    id_perfil uuid constraint perfil_id_fk references perfil on
      delete cascade,
    id_skill uuid constraint skill_id_fk references skill on
      delete cascade
)