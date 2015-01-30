
var DocProcessing = DocProcessing || {};

(function (dp) {

	dp.createNameSpace = function (namespaceString) {
		var parts = namespaceString.split('.'),
				parent = window,
				currentPart = '';

		for (var i = 0, length = parts.length; i < length; i++) {
			currentPart = parts[i];
			parent[currentPart] = parent[currentPart] || {};
			parent = parent[currentPart];
		}

		return parent;
	};

})(DocProcessing);

(function (dp) {
	var rootPath;
	dp.rootPath = rootPath;
}(DocProcessing));