﻿<%@ Page Language="C#" %>

<!DOCTYPE html>
<html>
<head>
<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.0-beta.17/angular.min.js"></script>
<title>Hello world from AngularJS</title>
</head>
<body>
<div ng-app>
<div>
<label>Name:</label>
<input type="text" ng-model="yourName" placeholder="Enter a
name here">
<hr>
<h1>Hello {{yourName}}!</h1>
</div>
</div>
</body>
</html>
