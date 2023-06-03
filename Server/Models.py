from sqlalchemy import Integer, String, Column, DateTime, REAL, ForeignKey, Text, BOOLEAN, Table
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import relationship


Base = declarative_base()

writers_books = Table("writers_books", Base.metadata,
                      Column("writer_id", Integer, ForeignKey("writers.id")),
                      Column("book_id", Integer, ForeignKey("books.id"))
                      )

class Writers(Base):
    __tablename__ = "writers"
    id = Column(Integer, primary_key=True)
    passport_number = Column(Integer)
    surname = Column(String)
    name = Column(String)
    middle_name = Column(String)
    address = Column(String)
    phone = Column(String)

    def __init__(self, data, session):
        self.id = data["id"]
        self.passport_number = data["passport_number"]
        self.surname = data["surname"]
        self.name = data["name"]
        self.middle_name = data["middle_name"]
        self.address = data["address"]
        self.phone = data["phone"]

    def to_json(self):
        result = {
            "id": self.id,
            "passport_number": self.passport_number,
            "surname": self.surname,
            "name": self.name,
            "middle_name": self.middle_name,
            "address": self.address,
            "phone": self.phone
        }
        return result

class Contracts(Base):
    __tablename__ = "contracts"
    id = Column(Integer, primary_key=True)
    id_writer = Column(Integer, ForeignKey("writers.id"))
    Writers = relationship(Writers)
    contract_number = Column(Integer)
    date_of_cons_contract = Column(DateTime)
    term_of_the_contract = Column(Integer)
    validy_of_the_contract = Column(BOOLEAN)
    date_of_terminition_contract = Column(DateTime)

    def __init__(self, data, session):
        self.id = data["id"]
        self.id_writer = data["id_writer"]["id"]
        self.Writers = session.query(Writers).get(data["id_writer"]["id"])
        self.contract_number = data["contract_number"]
        self.date_of_cons_contract = data["date_of_cons_contract"]
        self.term_of_the_contract = data["term_of_the_contract"]
        self.validy_of_the_contract = data["validy_of_the_contract"]
        self.date_of_terminition_contract = data["date_of_terminition_contract"]

    def to_json(self):
        result = {
            "id": self.id,
            "id_writer": self.Writers.to_json(),
            "contract_number": self.contract_number,
            "date_of_cons_contract": str(self.date_of_cons_contract),
            "term_of_the_contract": self.term_of_the_contract,
            "validy_of_the_contract": self.validy_of_the_contract,
            "date_of_terminition_contract": str(self.date_of_terminition_contract)
        }
        return result

class Books(Base):
    __tablename__ = "books"
    id = Column(Integer, primary_key=True)
    cipher_of_the_book = Column(Integer)
    name = Column(String)
    title = Column(String)
    circulation = Column(Integer)
    release_date = Column(DateTime)
    cost_price = Column(REAL)
    sale_price = Column(REAL)
    fee = Column(REAL)
    writers = relationship(Writers, secondary=writers_books)

    def __init__(self, data, session):
        self.id = data["id"]
        self.cipher_of_the_book = data["cipher_of_the_book"]
        self.name = data["name"]
        self.title = data["title"]
        self.circulation = data["circulation"]
        self.release_date = data["release_date"]
        self.cost_price = data["cost_price"]
        self.sale_price = data["sale_price"]
        self.fee = data["fee"]
        for writer in data["writers"]:
            self.writers.append(session.query(Writers).get(writer["id"]))

    def to_json(self):
        arr = []
        for writer in self.writers:
            arr.append(writer.to_json())

        result = {
            "id": self.id,
            "cipher_of_the_book": self.cipher_of_the_book,
            "name": self.name,
            "title": self.title,
            "circulation": self.circulation,
            "release_date": str(self.release_date),
            "cost_price": self.cost_price,
            "sale_price": self.sale_price,
            "fee": self.fee,
            "writers": arr
        }
        return result

class Customers(Base):
    __tablename__ = "customers"
    id = Column(Integer, primary_key=True)
    customer_name = Column(String)
    address = Column(String)
    phone = Column(String)
    full_name_customer = Column(String)

    def __init__(self, data, session):
        self.id = data["id"]
        self.customer_name = data["customer_name"]
        self.address = data["address"]
        self.phone = data["phone"]
        self.full_name_customer = data["full_name_customer"]

    def to_json(self):
        result = {
            "id": self.id,
            "customer_name": self.customer_name,
            "address": self.address,
            "phone": self.phone,
            "full_name_customer": self.full_name_customer
        }
        return result

class Orders(Base):
    __tablename__ = "orders"
    id = Column(Integer, primary_key=True)
    id_customer = Column(Integer, ForeignKey("customers.id"))
    Customer = relationship(Customers)
    order_number = Column(Integer)
    date_of_receipt_order = Column(DateTime)
    order_completion_date = Column(DateTime)
    id_book = Column(Integer, ForeignKey("books.id"))
    Book = relationship(Books)
    numbers_of_order = Column(Integer)

    def __init__(self, data, session):
        self.id = data["id"]
        self.id_customer = data["id_customer"]["id"]
        self.Customer = session.query(Customers).get(data["id_customer"]["id"])
        self.order_number = data["order_number"]
        self.date_of_receipt_order = data["date_of_receipt_order"]
        self.order_completion_date = data["order_completion_date"]
        self.id_book = data["id_book"]["id"]
        self.Book = session.query(Books).get(data["id_book"]["id"])
        self.numbers_of_order = data["numbers_of_order"]

    def to_json(self):
        result = {
            "id": self.id,
            "Customer": self.Customer.to_json(),
            "order_number": self.order_number,
            "date_of_receipt_order": str(self.date_of_receipt_order),
            "order_completion_date": str(self.order_completion_date),
            "Book": self.Book.to_json(),
            "numbers_of_order": self.numbers_of_order
        }
        return result