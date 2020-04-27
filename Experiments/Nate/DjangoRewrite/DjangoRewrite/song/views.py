from django.contrib.auth.decorators import login_required
from django.shortcuts import render

# Create your views here.
from django.views import View


class DefaultView(View):
    def get(self):
        pass


@login_required
class SearchBySongView(View):
    def get(self):
        pass


@login_required
class SearchByArtistView(View):
    def get(self):
        pass


@login_required
class AddToQueueView(View):
    def get(self):
        pass


@login_required
class QueueView(View):
    def get(self):
        pass