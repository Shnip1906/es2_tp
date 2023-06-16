create table cliente(
                        id_cliente uuid constraint idCliente_pk primary key default uuid_generate_v4(),
                        id_utilizador uuid,
                        id_utilizador_cliente uuid,
                        FOREIGN KEY (id_utilizador) REFERENCES utilizador (id_utilizador),
                            FOREIGN KEY (id_utilizador_cliente) REFERENCES utilizador (id_utilizador)
);

create table propostas(
      id_propostas uuid primary key default uuid_generate_v4(),
      nome_propostas varchar(100) not null,
      nTotalHoras int not null,
      descricao varchar(1000) not null,
      id_area_profissional uuid,
      id_cliente uuid,
      id_utilizador uuid,
      FOREIGN KEY (id_utilizador) REFERENCES utilizador (id_utilizador),
      FOREIGN KEY (id_area_profissional) REFERENCES areaProfissional (id_area_profissional),
      FOREIGN KEY (id_cliente) REFERENCES utilizador (id_utilizador)
);