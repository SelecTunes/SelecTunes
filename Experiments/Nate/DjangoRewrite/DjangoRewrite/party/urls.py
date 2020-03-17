from django.urls import path, include
from djangorewrite.party.views import DefaultView

urlpatterns = [
    path('', DefaultView.as_view()),

]
