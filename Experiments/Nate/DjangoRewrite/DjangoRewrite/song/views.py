from django.shortcuts import render

# Create your views here.
from django.views import View


class DefaultView(View):
    def get(self):
        pass


class SearchBySongView(View):
    def get(self):
        pass


class SearchByArtistView(View):
    def get(self):
        pass


class AddToQueueView(View):
    def get(self):
        pass


class QueueView(View):
    def get(self):
        pass