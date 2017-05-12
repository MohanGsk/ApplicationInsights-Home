"""DB init tests"""
# -*- coding: utf-8 -*-
import unittest
from pymongo import MongoClient
from datetime import datetime


class DBInitTestCase(unittest.TestCase):
    """DB Init test cases."""

    @classmethod
    def test_dbinit(cls):
        """Tests that DB is initialized correctly"""
        client = MongoClient("mongodb://unit_test_user:run@127.0.0.1")
        mongo_db = client.test_db
        result = mongo_db.restaurants.insert_one(
            {
                "address": {
                    "street": "2 Avenue",
                    "zipcode": "10075",
                    "building": "1480",
                    "coord": [-73.9557413, 40.7720266]
                },
                "borough": "Manhattan",
                "cuisine": "Italian",
                "grades": [
                    {
                        "date": datetime.strptime("2014-10-01", "%Y-%m-%d"),
                        "grade": "A",
                        "score": 11
                    },
                    {
                        "date": datetime.strptime("2014-01-16", "%Y-%m-%d"),
                        "grade": "B",
                        "score": 17
                    }
                ],
                "name": "Vella",
                "restaurant_id": "41704620"
            }
        )
        assert result.inserted_id > 0
