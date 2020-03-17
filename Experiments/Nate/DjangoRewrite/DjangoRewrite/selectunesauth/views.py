from django.shortcuts import render

# Create your views here.
from django.views import View


class DefaultView(View):
    def get(self):
        pass


class LoginView(View):
    def get(self):
        pass


class LogoutView(View):
    def get(self):
        pass


class GetCurrentUserEmail(View):
    def get(self):
        pass


class RegisterView(View):
    def get(self):
        pass


class BanView(View):
    def get(self):
        pass


class CallbackView(View):
    def get(self):
        pass
