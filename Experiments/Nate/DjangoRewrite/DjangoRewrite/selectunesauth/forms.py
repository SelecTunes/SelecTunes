from django import forms

from djangorewrite.party.models import RewriteUser


class AddRewriteUser(forms.ModelForm):

    class Meta:
        model = RewriteUser
        fields = ['email', 'password']
