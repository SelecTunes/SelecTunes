import os
import logging
from flask import Flask
from slack import WebClient
from slackeventsapi import SlackEventAdapter
import ssl as ssl_lib
import certifi

import config

app = Flask(__name__)
app.config.from_object("config.DevelopmentConfig")
# slack_events_adapter = SlackEventAdapter(config.SLACK_SIGNING_SECRET, "/slack/events", app)


