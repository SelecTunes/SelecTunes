all: install

install:
	bundle install

	mkdir -p /etc/st-update/

	cp -n -t /etc/st-update/ settings.yml app-settings.yml
	cp app.rb /usr/sbin/st-update

	chmod +x /usr/sbin/st-update