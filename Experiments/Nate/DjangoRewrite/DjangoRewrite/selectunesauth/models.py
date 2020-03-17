from django.db import models

from djangorewrite.party.models import Party
from djangorewrite.song.models import AccessAuthToken


class RewriteUser(models.Model):
    is_banned = models.BooleanField()
    party = models.ForeignKey(Party, on_delete=models.CASCADE)
    id_of_party = models.IntegerField()
    access_token = models.ForeignKey(AccessAuthToken, on_delete=models.CASCADE)


class InputModel(models.Model):
    email = models.TextField()
    password = models.CharField(max_length=255)


# class SpotifyIdentity(models.Model):
#     display_name = models.CharField()
#     email = models.CharField()
#     type = models.CharField()
#     product = models.CharField()
#
# class SpotifyLogin(models.Model):
#     code = models.CharField()
