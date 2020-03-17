from django.contrib import admin
from django.urls import path, include

urlpatterns = [
    path('admin/', admin.site.urls),
    path('api/song/', include('djangorewrite.song.urls')),
    path('api/party/', include('djangorewrite.party.urls')),
    path('api/auth', include('djangorewrite.selectunesauth.urls')),
]
