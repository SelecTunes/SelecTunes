from django.urls import path, include
from djangorewrite.song.views import DefaultView

urlpatterns = [
    path('', DefaultView.as_view()),

]
