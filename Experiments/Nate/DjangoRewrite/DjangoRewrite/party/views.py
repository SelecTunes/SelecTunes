from django.shortcuts import render
import json

# Create your views here.
from django.views import View

from djangorewrite.party.models import Party


class DefaultView(View):
    def get(self):
        pass


class JoinPartyView(View):
    def get(self):
        pass


class LeavePartyView(View):
    def get(self):
        pass


class DisbandPartyView(View):
    def get(self):
        pass


class MembersView(View):
    def get(self):
        pass

