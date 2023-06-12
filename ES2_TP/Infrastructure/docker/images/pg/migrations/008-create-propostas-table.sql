create table propostas(
                          id_propostas uuid primary key default uuid_generate_v4(),
                          nome_propostas varchar(100) not null,
                          nTotalHoras int not null,
                          descricao varchar(1000) not null,
                          id_area_profissional uuid,
                          FOREIGN KEY (id_area_profissional) REFERENCES areaProfissional (id_area_profissional)
)