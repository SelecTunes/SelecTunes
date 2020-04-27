from django.urls import path
from djangorewrite.party.views import *

urlpatterns = [
    path('', DefaultView.as_view()),
    path('joinparty/', JoinPartyView.as_view()),
    path('leaveparty/', LeavePartyView.as_view()),
    path('disbandparty/', DisbandPartyView.as_view()),
    path('members/', MembersView.as_view()),
]
