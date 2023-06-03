# PublishingCenter Client-Server
### Author: [Tolib](https://github.com/Tolib-Angle) Date: 03.06.2023
Console applications for database administration (Client: C#. Server: Python DateBase: PostgreSQL)

Necessary packages to run the program: JSON

The starting point of the programm: [Program.cs](https://github.com/Tolib-Angle/PublishingCenter_NHibernate/blob/main/Client/Program.cs)

BackUp database stored in file: [PublishingCenter](https://github.com/Tolib-Angle/PublishingCenter_NHibernate/blob/main/PublishingCenter.txt) (! _The date in the database is fictional_ !)

DateBase recoviry: `psql datebase_name < file_db_dump`

If you have any additional questions, write to the author: [Tolib](https://github.com/Tolib-Angle)

`Python engine = db.create_engine('postgresql+psycopg2://postgres:` _path to date base_ `')` in file [main.py](https://github.com/Tolib-Angle/PublishingCenter_NHibernate/blob/main/Server/main.py) line 10