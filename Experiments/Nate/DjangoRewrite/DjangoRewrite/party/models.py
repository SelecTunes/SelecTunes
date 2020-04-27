from django.db import models

from django.contrib.auth import get_user_model
RewriteUser = get_user_model()


class Party(models.Model):
    join_code = models.CharField(max_length=10)
    members = models.ForeignKey(RewriteUser, on_delete=models.CASCADE)
