import datetime
import json
import functools
import sqlite3
from contextlib import closing
from flask import Flask, g, make_response, render_template, request

DATABASE = 'megacat.db'
DEBUG = True
SECRET_KEY = 'megacat_dev'
# USERNAME = 'admin'
# PASSWORD = 'admin'

app = Flask(__name__)
app.config.from_object(__name__)


def as_json(f):
    @functools.wraps(f)
    def wrapped(*args, **kwargs):
        def _from_backend_object(obj):
            if isinstance(obj, datetime.datetime):
                return obj.isoformat(" ")
            else:
                json_encoder = json.JSONEncoder()
                return json_encoder.default(obj)

        result, http_status = f(*args, **kwargs)
        json_string = json.dumps(result, default=_from_backend_object)
        response = make_response(json_string, http_status)
        response.content_type = 'application/json'
        if 'application/json' not in request.headers.get('Accept'):
            response.content_type = 'text/plain'
        return response
    return wrapped


def connect_db():
    return sqlite3.connect(DATABASE)


def init_db():
    with closing(connect_db()) as db:
        with app.open_resource('schema.sql') as f:
            db.cursor().executescript(f.read())
        db.commit()


@app.before_request
def before_request():
    g.db = connect_db()


@app.teardown_request
def teardown_request(exception):
    g.db.close()


@app.route('/')
def start_page():
    loyalty = g.db.execute('SELECT loyalty FROM megacat').fetchone()[0]
    return render_template('start_page.html', params=dict(loyalty=loyalty))


@app.route('/loyalty/', methods=['GET'])
@as_json
def get_loyalty():
    cur = g.db.execute('SELECT loyalty FROM megacat')
    return {'loyalty': cur.fetchone()[0]}, 200


@app.route('/loyalty/', methods=['PUT'])
@as_json
def update_loyalty():
    new_loyalty = request.form.get('loyalty', 0, type=int)
    g.db.execute('UPDATE megacat SET loyalty=((SELECT loyalty FROM megacat)+(?))', [new_loyalty])
    g.db.commit()
    return 'success', 200


@app.route('/echo/', methods=['PUT'])
@as_json
def echo():
    "just for debug :)"
    return json.loads(request.data), 200


if __name__ == '__main__':
    init_db()
    app.run(host='0.0.0.0', port=42424)
