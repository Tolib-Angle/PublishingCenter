import datetime
import sqlalchemy as db
from flask import Flask, request
import json
from sqlalchemy import Integer, ForeignKey, String, Column, REAL, DateTime, Text, text
from sqlalchemy.orm import sessionmaker
from Models import *
from flask_swagger_ui import get_swaggerui_blueprint

engine = db.create_engine('postgresql+psycopg2://postgres:')

conn = engine.connect()

app = Flask(__name__)

SWAGGER_URL = '/swagger'
API_URL = '/static/swagger.json'
SWAGGERUI_BLUEPRINT = get_swaggerui_blueprint(
    SWAGGER_URL,
    API_URL,
    config={
        'app_name': "Seans-Python-Flask-REST-Boilerplate"
    }
)
app.register_blueprint(SWAGGERUI_BLUEPRINT, url_prefix=SWAGGER_URL)

Session = sessionmaker(bind=engine)
session = Session()

models = {
    "Writers": Writers,
    "Contracts": Contracts,
    "Books": Books,
    "Customers": Customers,
    "Orders": Orders
}

def query_list(class_, data):
    if data["StartId"] != -1:
        query = session.query(class_).filter(class_.id > data["StartId"]).order_by(class_.id)
    elif data["EndId"] != -1:
        query = session.query(class_).filter(class_.id < data["EndId"]).order_by(-class_.id)
    else:
        query = session.query(class_)

    if len(data["Condition"]) != 0:
        query = query.filter(text(data["Condition"]))

    if data["Count"] > 0:
        query = query.limit(data["Count"])

    result = []

    for entity in query:
        result.append(entity.to_json())
    return json.dumps(result)

def get_by_id(class_, id_):
    obj = session.query(class_).get(id_)
    return json.dumps(obj.to_json())


def upsert_entity(class_, data):
    print("call upsert entity 2")
    obj = class_(data, session)
    print("data: " + str(data))
    print("id: " + str(obj.id))
    if obj.id == -1:
        obj.id = None
    if obj.id is None:
        session.add(obj)
    else:
        session.merge(obj)

    session.commit()

    return str(obj.id)


def delete_entity(class_, id_):
    session.query(class_).filter(class_.id == id_).delete()
    session.commit()


@app.route("/api/list", methods=["POST"])
def get_list():
    try:
        data = request.get_json()
        if data["Table"] in models:
            return query_list(models[data["Table"]], data)
        return json.dumps([])
    except Exception as e:
        if hasattr(e, 'message'):
            return e.massege
        else:
            print(e)
            return "error"


@app.route("/api/get")
def get_entity():
    try:
        entity_id = int(request.args.get("id"))
        if request.args.get("table") in models:
            return get_by_id(models[request.args.get("table")], entity_id)
        return json.dumps({})
    except Exception as e:
        if hasattr(e, 'message'):
            return e.massege
        else:
            print(e)
            return "error"


@app.route("/api/upsert", methods=["POST"])
def upsert():
    print("upsert call!!!")
    try:
        data = request.get_json()
        if request.args.get("table") in models:
            print("call upsert entity")
            print(data)
            return upsert_entity(models[request.args.get("table")], data)
        return "-1"
    except Exception as e:
        if hasattr(e, 'message'):
            return e.massege
        else:
            print(e)
            return "error"


@app.route("/api/delete", methods=["POST"])
def delete():
    try:
        entity_id = int(request.args.get("id"))
        if request.args.get("table") in models:
            delete_entity(models[request.args.get("table")], entity_id)
        return "ok"
    except Exception as e:
        if hasattr(e, 'message'):
            return e.massege
        else:
            print(e)
            return "error"

@app.route("/api/get_count")
def get_count():
    try:
        if request.args.get("table") in models:
            count = str(session.query(models[request.args.get("table")]).count())
        return count
    except Exception as e:
        if hasattr(e, 'message'):
            return e.massege
        else:
            print(e)
            return "error"

def by_number(class_, number):
    return session.query(class_).order_by(class_.id).offset(number).limit(1).one().to_json()

@app.route("/api/get_by_number")
def get_by_number():
    try:
        if request.args.get("table") in models:
            obj = by_number(models[request.args.get("table")], int(request.args.get("number")) - 1)
            return obj
        return "not"
    except Exception as e:
        if hasattr(e, 'message'):
            return e.massege
        else:
            print(e)
            return "error"