import json
import mimetypes
import random

from django.shortcuts import render

from django.conf import settings
from django.contrib import messages
from django.contrib.auth.models import User
from django.core.exceptions import ObjectDoesNotExist
from django.http import Http404, HttpResponse
from django.shortcuts import redirect
from django.utils.translation import ugettext as _
from django.utils.html import mark_safe
from django.views.generic.base import View, TemplateResponseMixin
from django.views.generic.detail import SingleObjectMixin, SingleObjectTemplateResponseMixin
from django.views.generic.list import MultipleObjectMixin, MultipleObjectTemplateResponseMixin

from py_vapid import Vapid
from py_vapid.utils import b64urlencode


class IndexView(View):

    def get(self, request, context, *args, **kwargs):
        raise NotImplementedError("GET method not implemented on the view.")

    def post(self, request, context, *args, **kwargs):
        raise NotImplementedError("POST method not implemented on the view.")

    def put(self, request, context, *args, **kwargs):
        raise NotImplementedError("PUT method not implemented on the view.")

    def delete(self, request, context, *args, **kwargs):
        raise NotImplementedError("DELETE method not implemented on the view.")

    def head(self, request, context, *args, **kwargs):
        raise NotImplementedError("HEAD method not implemented on the view.")



 Create your views here.
