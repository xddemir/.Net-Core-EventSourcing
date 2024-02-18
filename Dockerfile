FROM ghcr.io/eventstore/eventstore@sha256:ab30bf2a2629e2c8710f8f7fdcb74df5466c6b3b2200c8e9ad8a797ed138012a
RUN apt-get update -y \
  && apt-get install -y openssl \
  && openssl req -x509 -sha256 -nodes -days 3650 -subj "/CN=eventstore.org" -newkey rsa:2048 -keyout eventstore.pem -out eventstore.csr \
  && openssl pkcs12 -export -inkey eventstore.pem -in eventstore.csr -out eventstore.p12 -passout pass: \
  && openssl pkcs12 -export -inkey eventstore.pem -in eventstore.csr -out eventstore.pfx -passout pass: \
  && mkdir -p /usr/local/share/ca-certificates \
  && cp eventstore.csr /usr/local/share/ca-certificates/eventstore.crt \
  && update-ca-certificates \
  && apt-get autoremove \
  && apt-get clean \
  && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*