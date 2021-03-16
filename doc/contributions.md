# Contributors guide

Pull requests are welcome!  A few guidelines...

## Source code

If you're planning a big change please let me know first via the Discussion forum or by raising an Feature issue.

Generally, please avoid NULLs - prefer string.Empty or other equivalent no-op values.

The main codebase (currently) deliberately avoids dependencies on external packages - hence why there is a home-built command-line parser.  Unless there's a compelling reason I'd prefer to keep the number of dependencies to zero.

Unit tests are always appreciated !

If you're adding a feature, it would be great if you could update the relevant documentation so people know about it.

## Scripts

If you're adding an extra nifty command for one shell please consider also adding it in the other shells if that's practical (DOS is pretty limited but it would be nice to keep PowerShell and Bash on an even footing). 

## Documentatation 

Yes please - more is better ! 