"""DB init tests"""
# -*- coding: utf-8 -*-
import unittest
from pymongo import MongoClient


class DBInitTestCase(unittest.TestCase):
    """DB Init test cases."""

    @classmethod
    def test_dbinit(cls):
        """Tests that DB is initialized correctly"""
        client = MongoClient('localhost', 27017)
