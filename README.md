# Ambiente de Desenvolvimento
- IDE
  - Visual Studio (Comunity 2022)
- Linguagem
  - C# (.Net 8.0)
- Interface Gráfica
  - WPF (Windows Presentation Fundadtion)
- Gerenciador de tarefas
  - Task.RUN
    - Método util para execução de tarefas em segundo plano, que retorna uma "Tarefa" que será consumida pelo operador "Await" dentro de uma método assíncrono, definido pelo marcador "Assync". Dessa forma é possível realizar tarefas paralelas sem congelar a UI.
    - Await: Serve para esperar a conclusão de uma Tarefa sem travar a interface (UI).
    - Assync: Palavra reservada utilizada na criação do método (logo após o modificador de acesso) para defini-lo como "assíncrono".

# Info de Desenvolvimento
## Objetivos

- Utilizar uma abordagem de processamento assíncrono
    - Permitindo múltiplas instâncias paralelamente

## Implementações

- [x]  Tela do servidor
- [x]  Tela do cliente
- [ ]  Tela de login
    - Ser a primeira tela, e depois dela vem a tela de cliente
- [ ]  Conexão TCP/IP
- [ ]  Banco de dados Teste
    - MongoDB
- [ ]  Pra entrar na tela de servidor é necessário um login específico :
    - Username → “Admin” / Password: (Senha específica)

## Tratamento de Exceções

- Client
- [x]  Não enviar mensagens sem servidor
- [x]  Não conectar em um servidor não iniciado
- [x]  Não desconectar de um servidor não iniciado
- [x]  Aviso de porta em uso
  
- Server
    - [x]  Não conectar se tiver endereço incorreto
    - [ ]  Não iniciar se já estiver iniciado no endereçoIP
    - [x]  Não desconectar se não houver antes uma conexão

## Bugs

- [ ]  Aviso de “Porta em uso” ao clicar em ‘Desconectar’

# Protótipos

## Telas
![diagrama-telas](https://github.com/user-attachments/assets/c71ac521-e9d3-4e86-aa66-9ba6101a5df4)

## Fluxograma
![fluxograma](https://github.com/user-attachments/assets/8c23c4d6-ee36-4ca2-ba0c-a72d9e7436bc)


