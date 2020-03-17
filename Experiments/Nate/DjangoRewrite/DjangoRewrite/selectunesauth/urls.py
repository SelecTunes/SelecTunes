from django.urls import path
from djangorewrite.selectunesauth.views import *

urlpatterns = [
    path('', DefaultView.as_view()),
    path('login/', LoginView.as_view()),
    path('logout/', LogoutView.as_view()),
    path('register/', RegisterView.as_view()),
    path('ban/', BanView.as_view()),
    path('callback/', CallbackView.as_view())
]
