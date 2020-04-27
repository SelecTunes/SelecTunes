from django.db import models

# Create your models here.


class AccessAuthToken(models.Model):
    access_token = models.TextField()
    token_type = models.TextField()
    refresh_token = models.TextField()
    scope = models.TextField()
    expires_in = models.IntegerField()
    created = models.DateTimeField()
