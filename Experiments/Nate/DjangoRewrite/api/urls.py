from django.conf.urls import url, include
from django.urls import re_path, path
from rest_framework import routers
from rest_framework.urlpatterns import format_suffix_patterns

API_TITLE = 'IScorE API'
API_DESCRIPTION = 'IScorE API Documentation'

router = routers.DefaultRouter(trailing_slash=False)

urlpatterns = [
    path('api-auth/', include('rest_framework.urls', namespace='rest_framework')),
]

urlpatterns = format_suffix_patterns(urlpatterns)
urlpatterns += router.urls
