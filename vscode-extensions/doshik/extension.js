const path = require('path');
const { workspace } = require('vscode');
const { LanguageClient } = require('vscode-languageclient');
const { Trace } = require('vscode-jsonrpc');

let client;

function activate(context) {

	const exePath = context.asAbsolutePath(
		path.join('server', 'DoshikLanguageServer.exe')
	);

	// If the extension is launched in debug mode then the debug server options are used
	// Otherwise the run options are used
	let serverOptions = {
		command: exePath
	};

	// Options to control the language client
	let clientOptions = {
        // Register the server for plain text documents
        documentSelector: [
            {
                pattern: '**/*.doshik',
            }
        ],
        synchronize: {
            // Synchronize the setting section 'doshikLanguageServer' to the server
            configurationSection: 'doshikLanguageServer',
            fileEvents: workspace.createFileSystemWatcher('**/*.doshik')
        },
	};

	// Create the language client and start the client.
	client = new LanguageClient(
		'doshikLanguageServer',
		'Doshik language server',
		serverOptions,
		clientOptions
	);

	//client.trace = Trace.Verbose;

	// Start the client. This will also launch the server
	client.start();
}

function deactivate() {
	if (!client) {
		return undefined;
	}
	
	return client.stop();
}

module.exports = {
	activate,
	deactivate
}
