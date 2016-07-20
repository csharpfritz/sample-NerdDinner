#!/bin/bash

npm install

bower install

gulp copy
gulp less
gulp uglify

dotnet restore