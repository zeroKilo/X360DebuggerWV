# X360DebuggerWV 

a remote debugger for the Xbox 360 debug monitor

<img src="https://i.imgur.com/n0LpDr9.png"></img>


requirements: xbdm and jrpc2 installed, xbox 360 neighborhood working


features:

General Infos: Name, IP Address, CPU Key, Kernel Version, Current Executable

File Browser: preview/save files, run xex executable

Modules: show base address in dump, show entry point in dump or cpu, explore sections

Memory Regions: browse all allocated memory sections, see which module sections are inside

Memory Dump: dump memory or write back to it with hex editor, load/save from/to file, hex pattern search

CPU: play, pause, step(if breakpoint was reached, not yet working for branching!), disassembly of ppc, current threads, current thread's registers, find end of a subfunction and export to XEXDecompiler 3 (integrated now)

Trace: Load/save/clear and see register changes

Overall Options: break on Module Load, break on Thread Create, record breakpoints to trace

[![Alt text](https://img.youtube.com/vi/p1H0QkF21Q4/0.jpg)](https://www.youtube.com/watch?v=p1H0QkF21Q4)
