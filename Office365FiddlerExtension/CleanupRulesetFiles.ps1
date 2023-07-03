#################################################################################
#
# The sample scripts are not supported under any Microsoft standard support 
# program or service. The sample scripts are provided AS IS without warranty 
# of any kind. Microsoft further disclaims all implied warranties including, without 
# limitation, any implied warranties of merchantability or of fitness for a particular 
# purpose. The entire risk arising out of the use or performance of the sample scripts 
# and documentation remains with you. In no event shall Microsoft, its authors, or 
# anyone else involved in the creation, production, or delivery of the scripts be liable 
# for any damages whatsoever (including, without limitation, damages for loss of business 
# profits, business interruption, loss of business information, or other pecuniary loss) 
# arising out of the use of or inability to use the sample scripts or documentation, 
# even if Microsoft has been advised of the possibility of such damages.
#
################################################################################# 

############################################################
#
#   Office 365 Fiddler Extension Ruleset Cleanup Script
#
#   v1.0    Jeremy Knight   7/3/2023  Initial version.
#
############################################################

# get Fiddler process
$fiddler = Get-Process fiddler -ErrorAction SilentlyContinue
if ($fiddler) {
  # try gracefully first
  $fiddler.CloseMainWindow()
  # kill after five seconds
  Sleep 5
  if (!$fiddler.HasExited) {
    $fiddler | Stop-Process -Force
  }
}
Remove-Variable fiddler

