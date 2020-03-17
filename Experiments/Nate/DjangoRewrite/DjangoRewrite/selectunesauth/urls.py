from django.urls import path, include
from djangorewrite.selectunesauth.views import DefaultView

urlpatterns = [
    path('', DefaultView.as_view()),

]
