from django.contrib.auth.decorators import login_required
from django.shortcuts import render
import json

from django.views import View


class DefaultView(View):
    def get(self):
        pass


@login_required
class JoinPartyView(View):
    def get(self):
        pass


@login_required
class LeavePartyView(View):
    def get(self):
        pass


@login_required
class DisbandPartyView(View):
    def get(self):
        pass


@login_required
class MembersView(View):
    def get(self):
        pass

