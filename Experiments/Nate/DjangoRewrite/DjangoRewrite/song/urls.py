from django.urls import path
from djangorewrite.song.views import *

urlpatterns = [
    path('', DefaultView.as_view()),
    path('queue/', QueueView.as_view()),
    path('searchbysong/', SearchBySongView.as_view()),
    path('searchbyartist/', SearchByArtistView.as_view()),
    path('addtoqueue/', AddToQueueView.as_view()),
]
