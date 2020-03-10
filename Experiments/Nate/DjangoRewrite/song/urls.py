from django.conf.urls import url, include
from django.contrib.auth.models import User
from django.urls import path
from rest_framework import routers, serializers, viewsets

# Routers provide an easy way of automatically determining the URL conf.
router = routers.DefaultRouter()

# Wire up our API using automatic URL routing.
# Additionally, we include login URLs for the browsable API.
urlpatterns = [
    path('/', include(router.urls)),
]
